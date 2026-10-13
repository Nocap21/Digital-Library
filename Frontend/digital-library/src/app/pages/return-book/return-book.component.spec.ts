import { TestBed } from '@angular/core/testing';
import { ReturnBookComponent } from './return-book.component';

describe('ReturnBookComponent', () => {
  it('creates the page component', async () => {
    await TestBed.configureTestingModule({ imports: [ReturnBookComponent] }).compileComponents();
    const fixture = TestBed.createComponent(ReturnBookComponent);
    expect(fixture.componentInstance).toBeTruthy();
  });
});
