import { TestBed } from '@angular/core/testing';
import { IssueBookComponent } from './issue-book.component';

describe('IssueBookComponent', () => {
  it('creates the page component', async () => {
    await TestBed.configureTestingModule({ imports: [IssueBookComponent] }).compileComponents();
    const fixture = TestBed.createComponent(IssueBookComponent);
    expect(fixture.componentInstance).toBeTruthy();
  });
});
