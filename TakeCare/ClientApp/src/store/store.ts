import { Commit, createStore } from "vuex";

interface State {
  authenticated: boolean;
  user: string;
}

export const store = createStore<State>({
  state: {
    authenticated: false,
    user: "",
  },
  mutations: {
    SET_AUTH: (state: { authenticated: boolean }, auth: boolean) => (state.authenticated = auth),
    SET_USER: (state: { user: string }, user: string) => (state.user = user),
  },
  actions: {
    setAuth: ({ commit }: { commit: Commit }, auth: boolean) => commit("SET_AUTH", auth),
    setUser: ({ commit }: { commit: Commit }, user: string) => commit("SET_USER", user),
  },
});
