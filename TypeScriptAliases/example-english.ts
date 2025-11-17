// Example usage of English type aliases
import { Text, Integer, TrueOrFalse, List, Optional, AsyncOperation } from './english-type-aliases';

// Basic usage
const name: Text = "John Doe";
const age: Integer = 30;
const isActive: TrueOrFalse = true;

// Collection usage
const hobbies: List<Text> = ["reading", "coding", "gaming"];
const scores: List<Integer> = [95, 87, 92];

// Optional values
const middleName: Optional<Text> = null;
const phoneNumber: Optional<Text> = "555-1234";

// Function with type aliases
function greet(personName: Text, personAge: Integer): Text {
    return `Hello, ${personName}! You are ${personAge} years old.`;
}

// Async function example
async function fetchUserData(userId: Integer): AsyncOperation<Text> {
    // Simulated async operation
    return `User data for ID: ${userId}`;
}

// Using the functions
console.log(greet(name, age));

fetchUserData(123).then((data: Text) => {
    console.log(data);
});

// Export for use in other files
export { name, age, isActive, hobbies };
