import axios from "axios";

// One HTTPS origin via Vite's proxy in development and the reverse proxy in production.
export const apiClient = axios.create({
  baseURL: "/api",
  withCredentials: true,
  headers: { Accept: "application/json" },
});

apiClient.interceptors.request.use(async (config) => {
  const method = (config.method || "get").toUpperCase();
  if (!["GET", "HEAD", "OPTIONS"].includes(method)) {
    // Fetch for the current identity; an anonymous token must not be reused after login.
    const response = await apiClient.get<{ token: string }>("/Login/Csrf");
    config.headers.set("X-CSRF-TOKEN", response.data.token);
  }
  return config;
});
