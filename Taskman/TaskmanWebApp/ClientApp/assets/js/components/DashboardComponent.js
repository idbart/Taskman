const DashboardComponent = {
	template: `
		<div id="dash-wrapper" class="fill-screen flex-row">

			<div id="tasks-wrapper" class="flex-column">
				<template v-if="inGroup">
					<div id="add-task-wrapper" class="span flex-column">
						<h3>Add a new task:</h3>
						<text-action btnText="Add" placeholder="Decribe task" v-on:submit="addTask"></text-action>
					</div>

					<div id="task-list-wrapper" class="span flex-column">
						<h3>Active Tasks:</h3>
						<div class="task-item flex-row" v-for="task in taskList" v-bind:key="task.id">
							<p>{{ task.description }}</p>
							<button v-if="task.status != 2" >Complete!</button>
						</div>
					</div>
				</template>
				<template v-else>
					<h3>Join a group to start tracking tasks!</h3>
				</template>
			</div>

			<div id="group-wrapper">
				<text-action btnText="Search" placeholder="search for a group" v-on:submit="searchForGroup"></text-action>
				
				<div v-if="searchingForGroup">
					<div v-for="result in searchResults">

					</div>
				</div>

				<div class="flex-row" v-else>
					<h3>Users in your group:</h3>
					<div class="user-card span" v-for="user in memberList" v-bind:key="user.id">
						{{ user.username }}
					</div>
				</div>
			</div>
		</div>
	`,
	props: [],
	data: function() {
		return {
			searchingForGroup: false,
			searchResults: []
		}
	},
	methods: {
		addTask: async function(newTask) {
			if (await DataAccess.createTask(this.$store.state.group.id, { description: newTask}))
			{
				await this.$store.dispatch('syncTaskList');
			}
		},
		searchForGroup: async function(name) {
			var group = await DataAccess.getGroup(name);
			this.searchForGroup = true;

			// what the hell is this?
			// fix it
			if (group == undefined)
			{

			}
			else
			{

			}
		}
	},
	computed: {
		taskList: function() {
			return this.inGroup ? this.$store.state.group.tasks : []; 
		},
		memberList: function() {
			return this.inGroup ? this.$store.state.group.members : [];
		},
		inGroup: function() {
			return this.$store.state.group != undefined;
		}
	},
	created: async function() {
		await this.$store.dispatch('syncState');

		if (this.$store.state.user == undefined)
		{
			this.$router.push('/login');
		}
	}
}