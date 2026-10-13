import { TestBed } from '@angular/core/testing';
import { AddBookComponent } from './add-book.component';

describe('AddBookComponent', () => {
  it('creates the page component', async () => {
    await TestBed.configureTestingModule({ imports: [AddBookComponent] }).compileComponents();
    const fixture = TestBed.createComponent(AddBookComponent);
    expect(fixture.componentInstance).toBeTruthy();
  });
});
