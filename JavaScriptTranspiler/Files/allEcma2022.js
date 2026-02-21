// ==========================================
// AST STRESS TEST: JAVASCRIPT
// Contains no host or built-in functions.
// ==========================================

// 1. Directives & Empty Statements
"use strict";
; // EmptyStatement

// 2. Variable Declarations & Primitives (Literal nodes)
var oldSchool = 1;
let intVal = 42;
let floatVal = 3.14159e-2;
let hexVal = 0xDEADBEEF;
let binVal = 0b101010;
let octVal = 0o755;
const bigIntVal = 9007199254740991n;
let strSingle = 'hello';
let strDouble = "world";
let isTrue = true, isFalse = false;
let nullVal = null;
let undefVal; // implicitly undefined
let regexLiteral = /^[a-z_][a-z0-9_]*$/gi;

// 3. Template Literals & Tagged Templates
function tagFunc(strings, ...values) {
    return strings[0]; // Avoid built-ins, just return the first string part
}
let templateStr = `Value is ${intVal} and ${strSingle}`;
let taggedTemplate = tagFunc`Tag this ${hexVal} value`;

// 4. Arrays & Spread Syntax (ArrayExpression)
let simpleArray = [1, 2, 3];
let sparseArray = [1, , 3];
let spreadArray = [0, ...simpleArray, 4];

// 5. Objects, Methods, Properties, Computed Keys (ObjectExpression)
let dynamicKey = "computed";
let complexObject = {
    standardKey: 100,
    "string-key": 200,
    [dynamicKey]: 300,
    oldSchool, // Property Shorthand
    method(a, b) { return a + b; },
    get accessor() { return this.standardKey; },
    set accessor(val) { this.standardKey = val; },
    ...spreadArray // Spread in object
};

// 6. Destructuring (ObjectPattern & ArrayPattern)
let { standardKey: extracted, [dynamicKey]: computedExtracted, ...restObj } = complexObject;
let [firstArr, secondArr = 99, ...restArr] = spreadArray; // Default values

// 7. Expressions & Operators
// Arithmetic (BinaryExpression)
let mathOp = (1 + 2) * 3 - 4 / 2 % 5 ** 2;
// Bitwise (BinaryExpression)
let bitOp = (intVal << 1) | (intVal >> 2) ^ (~intVal) & (intVal >>> 1);
// Logical (LogicalExpression)
let logicOp = ((isTrue && isFalse) || (intVal > 0)) ?? "fallback";
// Relational & Equality (BinaryExpression)
let relOp = (1 < 2) === (3 >= 3) !== (4 == "4");
let inOp = "standardKey" in complexObject;
// Unary (UnaryExpression)
let unaryOp = typeof intVal === "number" ? +strSingle : -floatVal;
let voidOp = void 0;
let deleteOp = delete complexObject["string-key"];
// Update (UpdateExpression)
let updateOp = intVal++;
updateOp = --intVal;
// Assignment (AssignmentExpression)
updateOp += 10;
updateOp **= 2;
updateOp &&= 5; // Logical assignment
// Sequence (SequenceExpression / Comma Operator)
let sequenceOp = (updateOp = 1, updateOp = 2, updateOp = 3);

// 8. Control Flow Statements
// IfStatement
if (intVal > 10) {
    updateOp = 1;
} else if (intVal < 0) {
    updateOp = -1;
} else {
    updateOp = 0;
}

// SwitchStatement
switch (updateOp) {
    case 1:
        intVal = 10;
        break;
    case 2:
    case 3: // Fallthrough
        intVal = 20;
        break;
    default:
        intVal = 0;
}

// Loops (While, DoWhile, For, ForIn, ForOf)
let loopCounter = 0;
while (loopCounter < 5) {
    loopCounter++;
}

do {
    loopCounter--;
} while (loopCounter > 0);

labeledLoop: for (let i = 0; i < 10; i++) {
    if (i === 2) continue;
    if (i === 8) break labeledLoop; // BreakStatement with Identifier
}

for (let key in complexObject) {
    loopCounter = 1;
}

for (let val of simpleArray) {
    loopCounter = val;
}

// 9. Error Handling (TryStatement, ThrowStatement, CatchClause)
try {
    // Cannot use `new Error()` as it's a built-in, so we throw a custom object
    throw { message: "Custom crash" };
} catch (err) {
    loopCounter = err.message;
} finally {
    loopCounter = 0;
}

// 10. Functions (FunctionDeclaration, ArrowFunctionExpression, IIFE)
function standardFunction(param1, param2 = "default", ...restParams) {
    return param1 + restParams[0]; // ReturnStatement
}

const arrowFunc = x => x * 2;
const arrowBlock = (x, y) => { return x + y; };

const iifeResult = (function(x) {
    return x * x;
})(10); // CallExpression

// 11. Generators (YieldExpression)
function* idMaker() {
    let index = 0;
    let injected = yield index++; // Yield can return a value passed to .next()
    yield* [1, 2, 3]; // Yield delegates to another iterable
    return injected;
}

// 12. Async / Await (AwaitExpression)
async function fetchMockData() {
    // Mocking a promise-like object to avoid built-in `new Promise()`
    let mockPromise = {
        then: function(resolve) { resolve(999); }
    };
    let result = await mockPromise;
    return result;
}

// 13. Classes (ClassDeclaration, MethodDefinition, PropertyDefinition)
class BaseClass {
    constructor(id) {
        this.id = id; // ThisExpression, MemberExpression
    }

    getBaseId() {
        return this.id;
    }
}

class SubClass extends BaseClass {
    #privateField = 42; // PrivateIdentifier
    static staticField = "GlobalState";

    constructor(id, name) {
        super(id); // Super
        this.name = name;

        // MetaProperty
        let isDirectNew = new.target === SubClass;
    }

    getPrivate() {
        return this.#privateField;
    }

    static getStatic() {
        return SubClass.staticField;
    }
}

// NewExpression
let instance = new SubClass(1, "Test");

// DebuggerStatement
debugger;