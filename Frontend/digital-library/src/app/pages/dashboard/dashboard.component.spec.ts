import { TestBed } from '@angular/core/testing';
import { DashboardComponent } from './dashboard.component';

describe('DashboardComponent', () => {
  it('creates the page component', async () => {
    await TestBed.configureTestingModule({ imports: [DashboardComponent] }).compileComponents();
    const fixture = TestBed.createComponent(DashboardComponent);
    expect(fixture.componentInstance).toBeTruthy();
  });
});
