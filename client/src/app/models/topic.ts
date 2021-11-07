export interface Topic {
    id: number;
    name: string;
    predmetID: string;
    studentName: string;
    created: string;
    createdDateTime: string;
    comments: Comment[];
}

export interface Comment {
    id: number;
    text: string;
    created: string;
    studentName: string;
    topicID: number;
    topic: Topic;
}