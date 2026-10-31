import { afterEach, describe, expect, it, vi } from "vitest";
import type { AxiosAdapter, InternalAxiosRequestConfig } from "axios";
import { apiClient } from "./client";
import { downloadExaminationPdf, getPatientExaminations } from "./examinationApi";
import { bookVisit } from "./visitsApi";
import type { VisitDto } from "@/models/VisitDto";

describe("API client security integration", () => {
  const originalAdapter = apiClient.defaults.adapter;
  afterEach(() => { apiClient.defaults.adapter = originalAdapter; vi.restoreAllMocks(); vi.unstubAllGlobals(); });

  it("fetches a CSRF token before each state-changing request", async () => {
    const requests: InternalAxiosRequestConfig[] = [];
    apiClient.defaults.adapter = (async (config) => {
      requests.push(config);
      return { data: config.url === "/Login/Csrf" ? { token: "csrf-fixture" } : {},
        status: 200, statusText: "OK", headers: {}, config };
    }) as AxiosAdapter;
    await bookVisit({} as VisitDto);
    expect(requests.map((r) => r.url)).toEqual(["/Login/Csrf", "/Visit/BookVisit"]);
    expect(requests[1].headers.get("X-CSRF-TOKEN")).toBe("csrf-fixture");
    expect(requests[1].withCredentials).toBe(true);
  });

  it("encodes an email with a plus sign as one query parameter", async () => {
    let observedUrl = "";
    apiClient.defaults.adapter = (async (config) => {
      observedUrl = apiClient.getUri(config);
      return { data: [], status: 200, statusText: "OK", headers: {}, config };
    }) as AxiosAdapter;
    await getPatientExaminations("patient+tag@example.invalid");
    const url = new URL(observedUrl, "https://localhost");
    expect(url.searchParams.get("patientEmail")).toBe("patient+tag@example.invalid");
    expect(url.searchParams.size).toBe(1);
  });

  it("downloads the saved PDF with credentials and a safe file name", async () => {
    const NativeURL = URL;
    vi.stubGlobal("URL", class extends NativeURL {
      static createObjectURL() { return "blob:local-test"; }
      static revokeObjectURL() { }
    });
    let filename = "";
    vi.spyOn(HTMLAnchorElement.prototype, "click").mockImplementation(function (this: HTMLAnchorElement) {
      filename = this.download;
    });
    apiClient.defaults.adapter = (async (config) => {
      expect(config.url).toBe("/Examination/42/pdf");
      expect(config.responseType).toBe("blob");
      expect(config.withCredentials).toBe(true);
      return { data: new Blob(["%PDF-"], { type: "application/pdf" }), status: 200, statusText: "OK", headers: {}, config };
    }) as AxiosAdapter;
    await downloadExaminationPdf(42);
    expect(filename).toBe("examination-42.pdf");
  });

  it("does not download an HTML response as a PDF", async () => {
    apiClient.defaults.adapter = (async (config) => ({
      data: new Blob(["<html>"], { type: "text/html" }), status: 200, statusText: "OK", headers: {}, config,
    })) as AxiosAdapter;
    await expect(downloadExaminationPdf(42)).rejects.toThrow("Invalid PDF response");
  });
});
