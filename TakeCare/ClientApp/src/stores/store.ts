import { defineStore } from "pinia";

export const useStore = defineStore("main", {
  state: () => ({
    isLogged: false,
    isLogging: false,
    isLoading: false,
    userMail: "",
    userToken: "",
    environment: "",
  }),
  actions: {
    setIsLogged(value: boolean) {
      this.isLogged = value;
    },
    setIsLogging(value: boolean) {
      this.isLogging = value;
    },
    setUserMail(value: string) {
      this.userMail = value;
    },
    setUserToken(value: string) {
      this.userToken = value;
    },
    setEnvironment(value: string) {
      this.environment = value;
    },
    setLoading(value: boolean) {
      this.isLoading = value;
    },
  },
});
