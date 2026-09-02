import { hitPages } from './http/walkthrough.navigation.menu.js';
import { uiWalkthrough } from './browser/walkthrough.navigation.menu.js';

export const options = {

  scenarios: {
    explore: {
      executor: 'ramping-vus',
      startVUs: 1,

      stages: [
        { duration: '30s', target: 20 },
        { duration: '5m', target: 20 },

        { duration: '30s', target: 50 },
        { duration: '5m', target: 50 },

        { duration: '30s', target: 80 },
        { duration: '5m', target: 80 },

        { duration: '30s', target: 100 },
        { duration: '5m', target: 100 },

        { duration: '2m', target: 200 },
        { duration: '10m', target: 200 },

        { duration: '2m', target: 400 },
        { duration: '10m', target: 400 },
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
