import http from 'k6/http'
import { check, sleep } from 'k6'

const APP_BASE_URL = 'https://localhost:10001'

export const options = {
    vus: 10,
    // iterations: 10
    // duration: "10s",
    stages: [
        { duration: '10s', target: 10 },
        { duration: '60s', target: 100 },
        { duration: '10s', target: 10 },
    ]
}

export default function () {

    let res = http.get(`${APP_BASE_URL}/Courses/Details/f3e9966c-467b-4b99-90ca-a29bae85ca94`)

    check(res, { 'success: course details page': r => r.status === 200 })

    // http.batch([
    //     ['GET', `${APP_BASE_URL}/Home/About`],
    //     ['GET', `${APP_BASE_URL}/Students`],
    //     ['GET', `${APP_BASE_URL}/Courses`],
    //     ['GET', `${APP_BASE_URL}/Instructors`],
    //     ['GET', `${APP_BASE_URL}/Departments`]
    // ]);

    sleep(1)
}
