import { Button, ButtonGroup, Card, CardContent, CardDescription, CardHeader, CardMeta, Icon, Image } from "semantic-ui-react";
import { Activity } from "../../app/layout/models/activity";


interface Props{
    activity : Activity;
    cancelSelectActivity: () => void;
}

export default function ActivityDetails({activity, cancelSelectActivity}: Props) {

    return (
        <Card fluid>
            <Image src={`/assets/categoryImages/${activity.category}.jpg`} />
            <CardContent>
                <CardHeader>{activity.title}</CardHeader>
                <CardMeta>
                    <span>{activity.date}</span>
                </CardMeta>
                <CardDescription>
                    {activity.description}
                </CardDescription>
            </CardContent>
            <CardContent extra>
                <ButtonGroup widths='2'>
                    <Button basic color="blue" content='Edit' />
                    <Button onClick={cancelSelectActivity} basic color="grey" content='Cancel' />
                </ButtonGroup>
            </CardContent>
        </Card>
    )
}