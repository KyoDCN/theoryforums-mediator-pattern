import { TestBed } from '@angular/core/testing';

import { SubforumBannerService } from './subforum-banner.service';

describe('SubforumBannerService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: SubforumBannerService = TestBed.get(SubforumBannerService);
    expect(service).toBeTruthy();
  });
});
