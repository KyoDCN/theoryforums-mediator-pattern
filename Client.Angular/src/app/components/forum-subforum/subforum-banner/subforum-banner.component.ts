import { Component, OnInit } from '@angular/core';
import { Subforum } from 'src/app/models/subforum';
import { SubforumBannerService } from 'src/app/services/subforum-banner.service';
import { Location } from '@angular/common';

@Component({
  selector: 'app-subforum-banner',
  templateUrl: './subforum-banner.component.html',
  styleUrls: ['./subforum-banner.component.css']
})
export class SubforumBannerComponent implements OnInit {
  subforum: Subforum;

  constructor(private location: Location, private subforumBannerService: SubforumBannerService) {
    this.subforum = new Subforum();
    this.subforumBannerService.subforum.subscribe((res: Subforum) => {
      this.subforum = res;
    });
  }

  ngOnInit() {
  }
}
