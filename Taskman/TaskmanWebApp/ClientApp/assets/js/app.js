"use strict";

// router
var router = VueRouter.createRouter({
	history: VueRouter.createWebHistory(),
	routes: [
		{ path: "/", component: DashboardComponent},
		{ path: "/login", component: LoginComponent},
		{ path: "/signup", component: SignUpComponent }
	]
});

// global store
var store = Vuex.createStore({
	state: function() {
		return {
			user: undefined,
			group: undefined
		};
	},
	getters: {

	},
	mutations: {
		setUser: function(state, payload) {
			state.user = payload.newUser;
		},
		setGroup: function(state, payload) {
			state.group = payload.newGroup;
		},
		setGroupTaskList: function(state, payload) {
			state.group.tasks = payload.taskList;
		}
	},
	// maybe make all these actions return Promises??
	actions: {
		// make a call to the /api/id server endpoint to sync the state of this client with the server
		syncUserState: async function(context, payload) {
			var user = await DataAccess.tryGetSelf();
			context.commit('setUser', { newUser: user });
		},
		// sync the state of this users group with the server
		syncUserGroupState: async function(context, payload) {
			var group = await DataAccess.getGroup();
			context.commit('setGroup', { newGroup: group });
		},

		// sync the state of the client with the server
		syncState: function(context, payload) {
			context.dispatch('syncUserState');
			context.dispatch('syncUserGroupState');
		}
	}
});

// spa root
var app = Vue.createApp({

	data: function() {
		return {
			
		}
	},
	methods: {

	},
	computed: {

	},
	mounted: async function() {

		await store.dispatch('syncState');

		if (store.state.user == undefined)
		{
			router.push('/login');
		}
	}
});

// add global components to app
app.component("text-action", TextActionComponent);

// tell the app to use the router and store
app.use(router);
app.use(store);

// mount the app on the #app element
app.mount("#app");