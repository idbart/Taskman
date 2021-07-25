const DashboardComponent = {
	template: `
		<div id="dash-wrapper" class="fill-screen flex-row" v-on:click="searchingForGroup = false">

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
					<div v-if="searchResults.length == 0">
						<h3>No groups found!</h3> 
					</div>
					<div v-else v-for="result in searchResults">
						<h3>{{ result.name }}</h3>
					</div>
				</div>

				<div class="flex-row" v-else>
					<h3>Users in your group: {{ group.name }}</h3>
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

			// tell the server to add a new task to this group
			if (await DataAccess.createTask(this.$store.state.group.id, { description: newTask}))
			{
				// if successful, reload the task list
				await this.$store.dispatch('syncTaskList');
			}
		},
		searchForGroup: async function(name) {
			// query the server for groups 
			var result = await DataAccess.searchGroupsByName(name);
			// tell the ui that the search is in progress
			this.searchingForGroup = true;

			if (result == undefined)
			{
				this.searchResults = [];
			}
			else
			{
				this.searchResults = result;
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
		},
		group: function() {
			return this.$store.state.group;
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