import React from 'react';
import { Bar } from 'react-chartjs-2';
import { Chart as ChartJS, CategoryScale, LinearScale, BarElement, Title, Tooltip, Legend } from 'chart.js';

// Register the chart components
ChartJS.register(CategoryScale, LinearScale, BarElement, Title, Tooltip, Legend);

// eslint-disable-next-line react/prop-types
const Analytics = ({ formData }) => {
    // Normalize answers and calculate frequency
    const calculateAnalytics = (formData) => {
        // eslint-disable-next-line react/prop-types
        return formData.map((question) => {
            // Filter out empty answers before processing
            const normalizedAnswers = question.answers
                .map((answer) => String(answer.value).toLowerCase()) // Convert to string and lowercase
                .filter((answer) => answer !== ''); // Exclude empty answers

            const frequency = {};

            // Count occurrences of each unique answer
            normalizedAnswers.forEach((answer) => {
                frequency[answer] = (frequency[answer] || 0) + 1;
            });

            // Sort the answers by frequency in descending order
            const sortedAnswers = Object.entries(frequency).sort((a, b) => b[1] - a[1]);

            return {
                question: question.title,
                topAnswers: sortedAnswers.slice(0, 3), // Get top 3 answers
                frequency,
            };
        });
    };

    // Generate chart data
    const generateChartData = (questionData) => {
        // Sort answers by frequency in descending order
        const sortedAnswers = Object.entries(questionData.frequency).sort((a, b) => b[1] - a[1]);

        const labels = sortedAnswers.map(([answer]) => answer); // Get the answers as labels
        const data = sortedAnswers.map(([, count]) => count); // Get the counts as data

        return {
            labels,
            datasets: [
                {
                    label: 'Answer Frequency',
                    data,
                    backgroundColor: [
                        'rgba(70, 130, 180, 0.5)',  // SteelBlue
                        'rgba(34, 139, 34, 0.5)',   // ForestGreen
                        'rgba(138, 43, 226, 0.5)',  // BlueViolet
                        'rgba(123, 104, 238, 0.5)', // MediumSlateBlue
                        'rgba(0, 191, 255, 0.5)',   // DeepSkyBlue
                    ], // Softer and less intense colors with transparency
                    borderColor: [
                        'rgba(70, 130, 180, 1)',  // SteelBlue
                        'rgba(34, 139, 34, 1)',   // ForestGreen
                        'rgba(138, 43, 226, 1)',  // BlueViolet
                        'rgba(123, 104, 238, 1)', // MediumSlateBlue
                        'rgba(0, 191, 255, 1)',   // DeepSkyBlue
                    ],
                    borderWidth: 1,
                },
            ],
            options: {
                responsive: true,
                plugins: {
                    tooltip: {
                        bodyFont: {
                            size: 14, // Tooltip font size
                        },
                    },
                },
                scales: {
                    x: {
                        ticks: {
                            font: {
                                size: 16, // Increase font size for the x-axis labels (text under the bars)
                            },
                        },
                    },
                    y: {
                        ticks: {
                            font: {
                                size: 16, // Increase font size for the y-axis labels
                            },
                        },
                    },
                },
            },
        };
    };

    const analytics = calculateAnalytics(formData);

    return (
        <div>
            {analytics.map((questionData, index) => (
                <div key={index}>
                    <hr/>
                    <h3 className='mt-5 mb-4 text-uppercase text-primary'>{questionData.question}</h3>
                    <div>
                        <h4>Top Answers:</h4>
                        <ul>
                            {questionData.topAnswers.map(([answer, count], idx) => (
                                <li key={idx}>
                                    {answer} - {count} times
                                </li>
                            ))}
                        </ul>
                    </div>
                    <Bar data={generateChartData(questionData)} />
                </div>
            ))}
        </div>
    );
};

export default Analytics;
