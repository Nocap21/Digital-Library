import { TestBed } from '@angular/core/testing';
import { ViewBooksComponent } from './view-books.component';

describe('ViewBooksComponent', () => {
  it('creates the page component', async () => {
    await TestBed.configureTestingModule({ imports: [ViewBooksComponent] }).compileComponents();
    const fixture = TestBed.createComponent(ViewBooksComponent);
    expect(fixture.componentInstance).toBeTruthy();
  });
});
