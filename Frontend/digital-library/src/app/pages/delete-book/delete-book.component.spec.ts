import { TestBed } from '@angular/core/testing';
import { DeleteBookComponent } from './delete-book.component';

describe('DeleteBookComponent', () => {
  it('creates the page component', async () => {
    await TestBed.configureTestingModule({ imports: [DeleteBookComponent] }).compileComponents();
    const fixture = TestBed.createComponent(DeleteBookComponent);
    expect(fixture.componentInstance).toBeTruthy();
  });
});
