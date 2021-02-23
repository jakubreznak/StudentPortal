export interface Topic {
    id: number;
    name: string;
    predmetID: string;
    studentName: string;
    created: string;
    comments: Comment[];
}

export interface Comment {
    id: number;
    text: string;
    created: string;
    studentName: string;
    topicID: number;
}