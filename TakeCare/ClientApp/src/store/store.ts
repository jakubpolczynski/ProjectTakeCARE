import { Commit, createStore } from "vuex";

interface State {
  authenticated: boolean;
  user: string;
  email: string;
}

const getDefaultState = (): State => {
  return {
    authenticated: localStorage.getItem("authenticated") === "true" || false,
    user: localStorage.getItem("user") || "",
    email: localStorage.getItem("email") || "",
  };
};

export const store = createStore<State>({
  state: getDefaultState(),
  mutations: {
    SET_AUTH: (state: State, auth: boolean) => {
      state.authenticated = auth;
      localStorage.setItem("authenticated", auth.toString());
    },
    SET_USER: (state: State, user: string) => {
      state.user = user;
      localStorage.setItem("user", user);
    },
    SET_EMAIL: (state: State, email: string) => {
      state.email = email;
      localStorage.setItem("email", email);
    },
  },
  actions: {
    setAuth: ({ commit }: { commit: Commit }, auth: boolean) => commit("SET_AUTH", auth),
    setUser: ({ commit }: { commit: Commit }, user: string) => commit("SET_USER", user),
    setEmail: ({ commit }: { commit: Commit }, email: string) => commit("SET_EMAIL", email),
  },
});
