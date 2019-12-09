import { TestBed } from '@angular/core/testing';

import { SubforumService } from './subforum.service';

describe('SubforumService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: SubforumService = TestBed.get(SubforumService);
    expect(service).toBeTruthy();
  });
});
