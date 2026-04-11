import { hitPages } from './http/walkthrough.navigation.menu.js';
import { uiWalkthrough } from './browser/walkthrough.navigation.menu.js';

export const options = {
  noVUConnectionReuse: true,

  scenarios: {
    http_pressure: {
      executor: 'constant-vus',
      vus: 50,
      duration: '20m',
      exec: 'hitPages',
    },
    ui_walkthrough: {
      executor: 'shared-iterations',
      vus: 1,
      iterations: 50,
      startTime: '5m',
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
