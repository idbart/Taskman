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

			if(this.username == "" || this.password == "")
			{
				this.message = "Cannot have empty fields!"
				return;
			}

			var result = await fetch("/api/login", {
				headers: {
					"Content-Type": "application/json" 
				},
				method: "POST",
				body: JSON.stringify({ username: this.username, password: this.password })
			});

			if(result.status == 200)
			{
				this.$router.push("/");
			}
			else
			{
				this.message = "Wrong username or password!"
			}
		}
	},
	computed: {

	}
};