export class Subforum {
    id: number;
    title: string;
    slug: string;
    description: string;
    icon: string;
}

export class SubforumAddDTO {
    forumId: number;
    title: string;
    description: string;
    icon: string;
}

export class SubforumUpdateDTO {
    title: string;
    description: string;
    icon: string;
}
