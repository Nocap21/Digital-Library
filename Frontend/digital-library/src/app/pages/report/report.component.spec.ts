import { TestBed } from '@angular/core/testing';
import { ReportComponent } from './report.component';

describe('ReportComponent', () => {
  it('creates the page component', async () => {
    await TestBed.configureTestingModule({ imports: [ReportComponent] }).compileComponents();
    const fixture = TestBed.createComponent(ReportComponent);
    expect(fixture.componentInstance).toBeTruthy();
  });
});
