import http from 'k6/http';
import { check } from 'k6';
import { parseHTML } from 'k6/html';
import { browser } from 'k6/browser';
import { fail } from 'k6';
import { expect } from 'https://jslib.k6.io/k6-testing/0.6.1/index.js';
import configs from './configs.js';

export const options = {
  scenarios: {
    http_pressure: {
      executor: 'constant-vus',
      vus: 100,
      duration: '10m',
      exec: 'hitPages',
    },
    ui_walkthrough: {
      executor: 'shared-iterations',
      vus: 1,
      iterations: 50,
      startTime: '3m',
      exec: 'uiWalkthrough',
      options: {
        browser: {
          type: 'chromium',
        },
      },
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
  const res = http.get(`${configs.rootUrl}`);
  const doc = parseHTML(res.body);
  const h1 = doc.find('main > div.jumbotron > h1');

  check(res, {
    'home 200': (r) => r.status === 200,
    'home has main heading': (r) => h1.text().trim() === 'Contoso University',
  });
}

function hitAbout() {
  const res = http.get(`${configs.rootUrl}/Home/About`);
  const doc = parseHTML(res.body);
  const h2 = doc.find('main > h2');

  check(res, {
    'about 200': (r) => r.status === 200,
    'about has main heading': (r) => h2.text().trim() === 'Student Body Statistics',
  });
}

function hitStudents() {
  const res = http.get(`${configs.rootUrl}/Students`);
  const doc = parseHTML(res.body);
  const title = doc.find('main > h2');

  check(res, {
    'students 200': (r) => r.status === 200,
    'students has main heading': (r) => title.text().trim() === 'Students',
  });
}

function hitCourses() {
  const res = http.get(`${configs.rootUrl}/Courses`);
  const doc = parseHTML(res.body);
  const title = doc.find('main > h2');

  check(res, {
    'courses 200': (r) => r.status === 200,
    'courses has main heading': (r) => title.text().trim() === 'Courses',
  });
}

function hitInstructors() {
  const res = http.get(`${configs.rootUrl}/Instructors`);
  const doc = parseHTML(res.body);
  const title = doc.find('main > h2');

  check(res, {
    'instructors 200': (r) => r.status === 200,
    'instructors has main heading': (r) => title.text().trim() === 'Instructors',
  });
}

function hitDepartments() {
  const res = http.get(`${configs.rootUrl}/Departments`);
  const doc = parseHTML(res.body);
  const title = doc.find('main > h1');

  check(res, {
    'departments 200': (r) => r.status === 200,
    'departments has main heading': (r) => title.text().trim() === 'Departments',
  });
}

export async function uiWalkthrough() {
  const page = await browser.newPage();

  try {
    await page.goto(configs.rootUrl);
    await expect.soft(page.locator('main > div.jumbotron > h1')).toHaveText('Contoso University');

    await page.goto(`${configs.rootUrl}/Home/About`);
    await expect.soft(page.locator('main > h2')).toHaveText('Student Body Statistics');

    await page.goto(`${configs.rootUrl}/Students`);
    await expect.soft(page.locator('main > h2')).toHaveText('Students');

    await page.goto(`${configs.rootUrl}/Courses`);
    await expect.soft(page.locator('main > h2')).toHaveText('Courses');

    await page.goto(`${configs.rootUrl}/Instructors`);
    await expect.soft(page.locator('main > h2')).toHaveText('Instructors');

    await page.goto(`${configs.rootUrl}/Departments`);
    await expect.soft(page.locator('main > h1')).toHaveText('Departments');
  } catch (error) {
    fail(`Browser iteration failed: ${error.message}`);
  } finally {
    await page.close();
  }
}