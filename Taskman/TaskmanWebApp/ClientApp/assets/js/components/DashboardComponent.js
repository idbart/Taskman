const DashboardComponent = {
	template: `
		<div id="dash-wrapper" class="fill-screen flex-row">

			<div id="tasks-wrapper" class="flex-column">
				<div id="add-task-wrapper" class="span flex-column">
					<h3>Add a new task:</h3>
					<text-action btnText="Add" placeholder="Decribe task" v-on:submit="addTask"></text-action>
				</div>

				<div id="task-list-wrapper" class="span flex-column">
					<h3>Active Tasks:</h3>
					<div class="task-item flex-row" v-for="task in tasks" v-bind:key="task.id">
						<p>{{ task.description }}</p>
						
					</div>
				</div>
			</div>

			<div id="group-wrapper">
			</div>
		</div>
	`,
	props: [],
	data: function() {
		return {
			tasks: []
		}
	},
	methods: {
		addTask: function(newTask) {
			this.tasks.push({ description: newTask });
			console.log(`added "${newTask}" to the task list`);
		}
	},
	computed: {
		
	}
}