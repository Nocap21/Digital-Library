import { TestBed } from '@angular/core/testing';
import { StudentService } from '../../services/student.service';
import { ViewStudentsComponent } from './view-students.component';

describe('ViewStudentsComponent', () => {
  let studentService: StudentService;

  beforeEach(async () => {
    await TestBed.configureTestingModule({ imports: [ViewStudentsComponent] }).compileComponents();
    studentService = TestBed.inject(StudentService);
  });

  it('creates the page component', () => {
    const fixture = TestBed.createComponent(ViewStudentsComponent);
    expect(fixture.componentInstance).toBeTruthy();
  });

  it('displays students obtained from StudentService', () => {
    studentService.addStudent({ rollNo: '1001', name: 'Asha Rao', branch: 'IT' });
    studentService.addStudent({ rollNo: '1002', name: 'Dev Kumar', branch: 'MECH' });

    const fixture = TestBed.createComponent(ViewStudentsComponent);
    fixture.detectChanges();

    const rows = fixture.nativeElement.querySelectorAll('tbody tr');
    expect(rows.length).toBe(2);
    expect(rows[0].textContent).toContain('1001');
    expect(rows[0].textContent).toContain('Asha Rao');
    expect(rows[0].textContent).toContain('IT');
    expect(rows[1].textContent).toContain('1002');
    expect(rows[1].textContent).toContain('Dev Kumar');
    expect(rows[1].textContent).toContain('MECH');
  });

  it('displays the empty state when there are no students', () => {
    const fixture = TestBed.createComponent(ViewStudentsComponent);
    fixture.detectChanges();

    expect(fixture.nativeElement.textContent).toContain('No students available.');
  });
});
