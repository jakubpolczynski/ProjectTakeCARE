import { beforeEach, describe, expect, it, vi } from "vitest";
import { store } from "@/store/store";

vi.mock("@/api/loginApi", () => ({ getUser: vi.fn(), logoutUser: vi.fn(), loginUser: vi.fn() }));
import { getUser } from "@/api/loginApi";
import router from "./index";

describe("session navigation", () => {
  beforeEach(() => {
    vi.mocked(getUser).mockReset();
    store.commit("SET_AUTH", false);
    store.commit("SET_USER", "");
    store.commit("SET_EMAIL", "");
  });

  it("clears a stale local session and sends the user to login", async () => {
    store.commit("SET_AUTH", true);
    store.commit("SET_USER", "Doctor");
    store.commit("SET_EMAIL", "stale@example.invalid");
    vi.mocked(getUser).mockRejectedValue(new Error("Unauthorized"));
    await router.push("/doctorvisits");
    expect(router.currentRoute.value.name).toBe("login");
    expect(store.state.authenticated).toBe(false);
    expect(store.state.email).toBe("");
  });

  it("uses the server role instead of a locally forged role", async () => {
    store.commit("SET_AUTH", true);
    store.commit("SET_USER", "Doctor");
    vi.mocked(getUser).mockResolvedValue({ data: { role: "Patient", email: "patient@example.invalid" } } as Awaited<ReturnType<typeof getUser>>);
    await router.push("/doctorvisits");
    expect(router.currentRoute.value.name).toBe("home");
    expect(store.state.user).toBe("Patient");
  });
});
