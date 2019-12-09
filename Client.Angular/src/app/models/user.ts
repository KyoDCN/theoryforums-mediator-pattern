import { BehaviorSubject } from 'rxjs';
export class User {
    id: number;
    displayName: string;
    email: string;
    avatarUrl: string;
    roles: string[];
}

export class LoginResponseDTO {
    token: string;
    user: User;
}
