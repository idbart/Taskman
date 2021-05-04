const LoginComponent = {
	template: `
		<div id="login-wrapper"> 
			<input type="text" placeholder="uername" />
			<input type="password" placeholder="password"/>
			<button v-on:click="tryLogin">login</button>
		</div>
	`,
	props: [],
	data: function() {
		return {

		}
	},
	methods: {
		tryLogin: function() {
			console.log("trying to login");
		},
	},
	computed: {

	}
};