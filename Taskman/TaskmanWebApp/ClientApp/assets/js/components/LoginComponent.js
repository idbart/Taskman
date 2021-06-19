const LoginComponent = {
	template: `
		<div id="login-outer-wrapper" class="fill-screen">
			<div id="login-inner-wrapper" class="flex-column twin center">
				<div id="login-banner" class="span"> {{ message }} </div>
				<input type="text" placeholder="uername" v-model="username"/>
				<input type="password" placeholder="password" v-model="password"/>
				<button v-on:click="tryLogin">login</button>
			</div>
		</div>
	`,
	props: [],
	data: function() {
		return {
			message: "",
			username: "",
			password: ""
		}
	},
	methods: {
		tryLogin: async function() {
			
			// make sure the user input both a username and a password
			if(this.username == "" || this.password == "")
			{
				this.message = "Cannot have empty fields!"
				return;
			}

			// sent out ajax request to server login endpoint with username and password
			var result = await fetch("/api/login", {
				headers: {
					"Content-Type": "application/json" 
				},
				method: "POST",
				body: JSON.stringify({ username: this.username, password: this.password })
			});

			// if the server response comes back with a 200 status, the login was sucessfull and user data is in the body
			// put user data in the global state and send user to the dashboard
			if(result.status == 200)
			{
				this.$store.commit('setUser', { newUser: result.json() });
				this.$router.push('/');
			}
			// if not a 200 response, login was unsucessfull
			else
			{
				this.message = "Wrong username or password!"
			}
		}
	},
	computed: {

	},
	mounted: function() {
		// if the user is already logged in, send them to the dashboard
		if (this.$store.state.user != undefined)
		{
			this.$router.push('/');
		}
	}
};