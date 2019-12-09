import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { ForumService } from '../services/forum.service';

@Injectable()
export class ForumResolver implements Resolve<any> {

    constructor(private forumService: ForumService) { }

    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        return this.forumService.getForums();
    }
}
