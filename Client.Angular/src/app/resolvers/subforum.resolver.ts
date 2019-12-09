import { Observable } from 'rxjs';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { SubforumService } from '../services/subforum.service';
import { Injectable } from '@angular/core';
import { Subforum } from '../models/subforum';
import { Thread } from '../models/thread';
import { ThreadService } from '../services/thread.service';

@Injectable()
export class SubforumResolver implements Resolve<any> {
    subforum: Observable<Subforum>;
    threads: Observable<Thread>;

    constructor(private subforumService: SubforumService, private threadService: ThreadService) {
    }

    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        const subforumId = parseInt(route.paramMap.get('id'), 10);
        this.subforum = this.subforumService.getSubforum(subforumId);
        this.threads = this.threadService.getThreads(subforumId);
        return { subforum: this.subforum, threadlist: this.threads, subforumId };
    }
}
