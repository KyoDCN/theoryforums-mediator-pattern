import { Author } from './author';

export class Forum {
    id: number;
    title: string;
    slug: string;
    description: string;
    subforums: Array<
        {
            id: number;
            title: string;
            slug: string;
            description: string;
            icon: string;
            latestReply: {
                threadId: number;
                threadTitle: string;
                threadSlug: string;
                postReplyDate: Date;
                author: Author
            };
        }
    >;

}
