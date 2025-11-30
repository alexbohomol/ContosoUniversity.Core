import http from 'k6/http';
import { check } from 'k6';
import { parseHTML } from 'k6/html';

const BASE_URL = __ENV.BASE_URL || 'http://contoso-mnlth-alb-963508264.eu-central-1.elb.amazonaws.com';

export const options = {
  scenarios: {
    api_load: {
      executor: 'constant-vus',
      vus: 100,
      duration: '10m',
      exec: 'hitPages',
    },
  },
  thresholds: {
    http_req_failed: ['rate == 0'],
    checks: ['rate == 1'],
  },
};

export function hitPages() {
  hitHome();
  hitAbout();
  hitStudents();
  hitCourses();
  hitInstructors();
  hitDepartments();
}

function hitHome() {
  const res = http.get(`${BASE_URL}`);
  const doc = parseHTML(res.body);
  const h1 = doc.find('main > div.jumbotron > h1');

  check(res, {
    'home 200': r => r.status === 200,
    'home has main heading': r => h1.text().trim() === 'Contoso University',
  });
}

function hitAbout() {
  const res = http.get(`${BASE_URL}/Home/About`);
  const doc = parseHTML(res.body);
  const h2 = doc.find('main > h2');

  check(res, {
    'about 200': r => r.status === 200,
    'about has main heading': r => h2.text().trim() === 'Student Body Statistics',
  });
}

function hitStudents() {
  const res = http.get(`${BASE_URL}/Students`);
  const doc = parseHTML(res.body);
  const title = doc.find('main > h2');

  check(res, {
    'students 200': r => r.status === 200,
    'students has main heading': r => title.text().trim() === 'Students',
  });
}

function hitCourses() {
  const res = http.get(`${BASE_URL}/Courses`);
  const doc = parseHTML(res.body);
  const title = doc.find('main > h2');

  check(res, {
    'courses 200': r => r.status === 200,
    'courses has main heading': r => title.text().trim() === 'Courses',
  });
}

function hitInstructors() {
  const res = http.get(`${BASE_URL}/Instructors`);
  const doc = parseHTML(res.body);
  const title = doc.find('main > h2');

  check(res, {
    'instructors 200': r => r.status === 200,
    'instructors has main heading': r => title.text().trim() === 'Instructors',
  });
}

function hitDepartments() {
  const res = http.get(`${BASE_URL}/Departments`);
  const doc = parseHTML(res.body);
  const title = doc.find('main > h1');

  check(res, {
    'departments 200': r => r.status === 200,
    'departments has main heading': r => title.text().trim() === 'Departments',
  });
}
