import { Delta } from 'quill/core';
import { Author } from './author';

export class Post {
    id: number;
    content: Delta;
    createdOn: Date;
    edited: boolean;
    lastEditDate: Date;
    author: Author;
}

export class PostAddDTO {
    threadId: number;
    content: Delta;
}

export class PostUpdateDTO {
    content: Delta;
}
