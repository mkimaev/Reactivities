import { makeAutoObservable } from "mobx";
import { Activity } from "../layout/models/activity";
import agent from "../api/agent";

export default class ActivityStore{
    activities: Activity[] = [];
    selectedActivity: Activity | null = null;
    editMode = false;
    loading = false;
    loadingInitial = false;

    constructor(){
        makeAutoObservable(this);
    }

    loadActivities = async () => {
        this.loadingInitial = true;
        try {
            const activities = await agent.Activities.list();

            activities.forEach(activity => {
                activity.date = activity.date.split('T')[0];
                this.activities.push(activity);
              });

            this.loadingInitial = false;
        } catch (error) {
            console.log(error);
            this.loadingInitial = false;
        }
    }
}

/*

agent.Activities.list().then(response => {
      let activities: Activity[] = [];
      response.forEach(activity => {
        activity.date = activity.date.split('T')[0];
        activities.push(activity);
      })
      setActivities(response);
      setLoading(false);
    })
*/