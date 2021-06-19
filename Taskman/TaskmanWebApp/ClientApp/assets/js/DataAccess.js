const DataAccess = {
	tryGetSelf: function() {
		return new Promise(async (resolve, reject) => {

			var result = await fetch('/api/id');

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
	}
};