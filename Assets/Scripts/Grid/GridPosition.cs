using System;

/* GridPosition struct represents a specific position in the grid with x and z coordinates. 
 * provides methods for equality comparison, hashing, and string representation. 
 * used to specify and compare positions within the grid.*/

public struct GridPosition : IEquatable<GridPosition>{
    public int x;
    public int z;

    // Constructor to initialize a GridPosition
    public GridPosition(int x , int z) {
        this.x = x;
        this.z = z;
    }

    // Override ToString method to provide a string representation of the GridPosition
    public override string ToString() {
        return $"x: {x}; z: {z}";  // same as return $"x: {x}; z: {z}";
    }



    // Equality operator
    // compare two GridPosition instances. It returns true if both x and z coordinates are equal for the two instances
    public static bool operator ==(GridPosition a, GridPosition b) {
        return a.x == b.x && a.z == b.z;
    }
    // Inequality operator
    // compare two GridPosition instances. It returns true if either the x or z coordinates are not equal for the two instances.
    public static bool operator !=(GridPosition a, GridPosition b) {
        return !(a == b);
    }
    /* 
    When you use the == or != operators to compare two GridPosition instances, the compiler automatically calls the overloaded 
    operator method, passing the two instances as arguments.
        So we can use:                             instead of
    if (newGridPosition != gridPosition) { ... }      
    if (newGridPosition.x != gridPosition.x || newGridPosition.z != gridPosition.z) { ... }
     */



    // These were generateb by vs studio to get rid of warnings:

    // Override Equals method for object comparison - compare GridPosition instances based on their x and z coordinates
    public override bool Equals(object obj) {
        return obj is GridPosition position &&
               x == position.x &&
               z == position.z;
    }

    // Implement IEquatable interface - type-safe Equals method for comparing GridPosition instances, leveraging the overloaded == operator
    public bool Equals(GridPosition other) {
        return this == other;
    }

    // Generates a hash code based on the x and z coordinates, ensuring that equal instances produce the same hash code, which is essential
    // for efficient operation of hash-based collections
    public override int GetHashCode() {
        return HashCode.Combine(x, z);
    }


    public static GridPosition operator +(GridPosition a, GridPosition b) {
        return new GridPosition(a.x + b.x, a.z + b.z);
    }

    public static GridPosition operator -(GridPosition a, GridPosition b) {
        return new GridPosition(a.x - b.x, a.z - b.z);
    }

}