const DataAccess = {
	tryGetSelf: function() {
		return new Promise(async (resolve, reject) => {

			var result = await fetch('api/id');

			if(result.status == 200)
			{
				resolve(result.json());
			}
			else
			{
				resolve(undefined);
			}
		});
	},
	getGroup: function(id) {
		return new Promise(async (resolve, reject) => {
			
			var result = await fetch(id ? `api/groups/${id}` : 'api/groups');

			if (result.status == 200)
			{
				resolve(result.json());
			}
			else
			{
				resolve(undefined);
			}
		});
	},
	searchGroupsByName: function(pattern) {
		return new Promise(async (resolve, reject) => {
			var result = await fetch(`api/groups/search/${pattern}`);

			if (result.status == 200)
			{
				resolve(result.json());
			}
			else
			{
				resolve(undefined);
			}
		});
	},
	getGroupTasks: function(gid) {
		return new Promise( async (resolve, reject) => {
			if(gid)
			{
				var result = await fetch(`api/groups/${gid}/tasks`);

				if (result.status == 200)
				{
					resolve(result.json());
				}
				else
				{
					reject(result.json());
				}
			}
			else
			{
				reject("must provide group id");
			}
		});
	},
	createTask: function(gid, task) {
		return new Promise(async (resolve, reject) => {

			var result = await fetch(`api/groups/${gid}/tasks/create`, {
				headers: {
					"Content-Type": "application/json"
				},
				method: "POST",
				body: JSON.stringify(task)
			});
	
			// just return true is response in 200 and false if not for now
			// eventually this should return more detail if response is not 200
			if (result.status == 200)
			{
				resolve(true);
			}
			else
			{
				resolve(false);
			}
		});
	}
};