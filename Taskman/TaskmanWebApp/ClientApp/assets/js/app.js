
// router
var router = VueRouter.createRouter({
	history: VueRouter.createWebHistory(),
	routes: [
		{ path: "/", component: DashboardComponent},
		{ path: "/login", component: LoginComponent},
		{ path: "/signup", component: SignUpComponent }
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

// add global components to app
app.component("text-action", TextActionComponent);

// tell the app to use the router
app.use(router);
// mount the app on the #app element
app.mount("#app");