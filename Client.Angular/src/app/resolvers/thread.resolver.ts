import { Subforum } from 'src/app/models/subforum';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Injectable } from '@angular/core';
import { ThreadService } from '../services/thread.service';
import { Thread } from '../models/thread';
import { Observable } from 'rxjs';
import { PostService } from '../services/post.service';
import { Post } from '../models/post';
import { SubforumService } from '../services/subforum.service';

@Injectable()
export class ThreadResolver implements Resolve<any> {
    thread: Observable<Thread>;
    posts: Observable<Array<Post>>;

    constructor(private threadService: ThreadService, private postService: PostService, private subforumService: SubforumService) { }

    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        const routeId: number = parseInt(route.paramMap.get('id'), 10);

        this.thread = this.threadService.getThread(routeId);
        this.posts = this.postService.getPosts(routeId);

        this.threadService.incrementThreadViewCount(routeId);

        return { thread: this.thread, posts: this.posts };
    }
}
