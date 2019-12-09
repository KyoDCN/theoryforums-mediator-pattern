// Modules
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ModalModule, BsDropdownModule } from 'ngx-bootstrap';

// Components
import { AppComponent } from './app.component';
import { ForumPostComponent } from './components/forum-post/forum-post.component';
import { ForumThreadComponent } from './components/forum-thread/forum-thread.component';
import { ForumSubforumComponent } from './components/forum-subforum/forum-subforum.component';
import { ForumPageComponent } from './components/forum-page/forum-page.component';
import { ForumMainComponent } from './components/forum-main/forum-main.component';
import { ForumModalComponent } from './components/forum-main/forum-modal/forum-modal.component';
import { SubforumModalComponent } from './components/forum-subforum/subforum-modal/subforum-modal.component';
import { ThreadModalComponent } from './components/forum-thread/thread-modal/thread-modal.component';
import { UserProfileComponent } from './components/user-profile/user-profile.component';
import { NavComponent } from './components/nav/nav.component';
import { NavModalComponent } from './components/nav/nav-modal/nav-modal.component';

// Services
import { ForumService } from './services/forum.service';
import { SubforumService } from './services/subforum.service';
import { ThreadService } from './services/thread.service';
import { PostService } from './services/post.service';

// Resolvers
import { ForumResolver } from './resolvers/forum.resolver';
import { SubforumResolver } from './resolvers/subforum.resolver';
import { ThreadResolver } from './resolvers/thread.resolver';
import { RegisterComponent } from './components/register/register.component';
import { RichTextEditorComponent } from './components/rich-text-editor/rich-text-editor.component';
import { SubforumBannerComponent } from './components/forum-subforum/subforum-banner/subforum-banner.component';
import { AlertBannerComponent } from './components/forum-main/alert-banner/alert-banner.component';

@NgModule({
  declarations: [
    AppComponent,
    ForumPostComponent,
    ForumThreadComponent,
    ForumSubforumComponent,
    ForumPageComponent,
    ForumMainComponent,
    ForumModalComponent,
    SubforumModalComponent,
    ThreadModalComponent,
    UserProfileComponent,
    NavComponent,
    NavModalComponent,
    RegisterComponent,
    RichTextEditorComponent,
    SubforumBannerComponent,
    AlertBannerComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    ModalModule.forRoot(),
    BsDropdownModule.forRoot(),
  ],
  providers: [
    ForumService,
    SubforumService,
    ThreadService,
    PostService,
    ForumResolver,
    SubforumResolver,
    ThreadResolver
  ],
  entryComponents: [
    ForumModalComponent,
    SubforumModalComponent,
    ThreadModalComponent,
    NavModalComponent
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
