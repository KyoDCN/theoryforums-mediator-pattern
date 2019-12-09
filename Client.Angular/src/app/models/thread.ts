import { Author } from './author';
import { Subforum } from './subforum';

export class Thread {
    id: number;
    title: string;
    slug: string;
    author: Author = new Author();
    content: string;
    createdOn: Date;
    edited: boolean;
    lastEditDate: Date;
    replies: number;
    views: number;
    subforum: Subforum;
}

export class ThreadAddDTO {
    subforumId: number;
    title: string;
    content: string;
}

export class ThreadUpdateDTO {
    title: string;
    content: string;
}
