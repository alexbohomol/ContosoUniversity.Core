import { hitPages } from './http.pressure.js';
import { uiWalkthrough } from './ui.walkthrough.js';

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

export { hitPages, uiWalkthrough };