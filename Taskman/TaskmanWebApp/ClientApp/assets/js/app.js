
// router
var router = VueRouter.createRouter({
	history: VueRouter.createWebHistory(),
	routes: [
		{ path: "/", component: DashboardComponent},
		{ path: "/login", component: LoginComponent},
	]
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

	}
});

// tell the app to use the router
app.use(router);
// mount the app on the #app element
app.mount("#app");