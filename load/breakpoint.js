import { hitPages } from './http/walkthrough.navigation.menu.js';
import { uiWalkthrough } from './browser/walkthrough.navigation.menu.js';

export const options = {

  scenarios: {
    breakpoint: {
      executor: 'ramping-vus',
      stages: [
        { duration: '50m', target: 2500 }
      ],
      exec: 'hitPages',
    },
  },

  thresholds: {
    http_req_failed: ['rate == 0'],
    checks: ['rate == 1'],
  },
};

export { hitPages, uiWalkthrough };
