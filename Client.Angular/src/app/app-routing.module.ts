import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ForumSubforumComponent } from './components/forum-subforum/forum-subforum.component';
import { ForumThreadComponent } from './components/forum-thread/forum-thread.component';
import { ForumMainComponent } from './components/forum-main/forum-main.component';
import { ForumResolver } from './resolvers/forum.resolver';
import { SubforumResolver } from './resolvers/subforum.resolver';
import { ThreadResolver } from './resolvers/thread.resolver';
import { UserProfileComponent } from './components/user-profile/user-profile.component';
import { RegisterComponent } from './components/register/register.component';

const routes: Routes = [
  { path: '', redirectTo: '/forum', pathMatch: 'full' },
  { path: 'forum', component: ForumMainComponent, resolve: { forum: ForumResolver } },
  { path: 'subforum/:id/:slug', component: ForumSubforumComponent, resolve: { subforum: SubforumResolver } },
  { path: 'thread/:id/:slug', component: ForumThreadComponent, resolve: { thread: ThreadResolver } },
  { path: 'user/:id', component: UserProfileComponent },
  { path: 'register', component: RegisterComponent },
  { path: '**', redirectTo: '/forum' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
