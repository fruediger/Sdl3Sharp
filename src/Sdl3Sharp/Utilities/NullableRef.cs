using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3Sharp.Utilities;

/// <summary>
/// Represents a explicitly nullable reference
/// </summary>
/// <typeparam name="T">The type of value the <see cref="NullableRef{T}"/> represents a reference of</typeparam>
/// <param name="target">An actual reference to the value of type <typeparamref name="T"/> the <see cref="NullableRef{T}"/> should represent a reference to</param>
/// <remarks>
/// <para>
/// To construct a <see cref="NullableRef{T}"/> that represents a <c><see langword="null"/></c>-reference, use <see cref="Null"/> or <c><see langword="default"/>(<see cref="NullableRef{T}">NullableRef</see>&lt;<typeparamref name="T"/>&gt;)</c>.
/// </para>
/// </remarks>
[StructLayout(LayoutKind.Sequential)]
[method: MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
public readonly ref struct NullableRef<T>(ref T target) : IEquatable<NullableRef<T>>, IEquatable<NullableRefReadOnly<T>>
{
	/// <summary>
	/// Gets a <see cref="NullableRef{T}"/> that represents a <c><see langword="null"/></c>-reference
	/// </summary>
	/// <value>
	/// A <see cref="NullableRef{T}"/> that represents a <c><see langword="null"/></c>-reference
	/// </value>
	/// <remarks>
	/// <para>
	/// Do not try the dereference the returned reference from a call to <see cref="GetReferenceOrNull"/> on the resulting value of this property!
	/// </para>
	/// </remarks>
	public static NullableRef<T> Null { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => new(ref Unsafe.NullRef<T>()); }

	private readonly ref T mTarget = ref target;

	/// <summary>
	/// Gets a value indicating whether this <see cref="NullableRef{T}"/> does <em>not</em> represent a <c><see langword="null"/></c>-reference and the <see cref="Target"/> property can be safely dereferenced
	/// </summary>
	/// <value>
	/// A value indicating whether this <see cref="NullableRef{T}"/> does <em>not</em> represent a <c><see langword="null"/></c>-reference
	/// </value>
	public readonly bool HasTarget { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => !IsNull; }

	/// <summary>
	/// Gets a value indicating whether this <see cref="NullableRef{T}"/> does represent a <c><see langword="null"/></c>-reference 
	/// </summary>
	/// <value>
	/// A value indicating whether this <see cref="NullableRef{T}"/> does represent a <c><see langword="null"/></c>-reference 
	/// </value>
	/// <remarks>
	/// <para>
	/// If the value of this property is <c><see langword="true"/></c>, the <see cref="Target"/> property can't be safely dereferenced.
	/// </para>
	/// </remarks>
	public readonly bool IsNull { [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)] get => Unsafe.IsNullRef(ref mTarget); }

	/// <summary>
	/// Gets an actual reference to the value of type <typeparamref name="T"/> this <see cref="NullableRef{T}"/> references
	/// </summary>
	/// <value>
	/// An actual reference to the value of type <typeparamref name="T"/> this <see cref="NullableRef{T}"/> references
	/// </value>
	/// <remarks>
	/// <para>
	/// If the value of the <see cref="HasTarget"/> property is <c><see langword="false"/></c> or the value of the <see cref="IsNull"/> property is <c><see langword="true"/></c>,
	/// accessing this property will throw an <see cref="InvalidOperationException"/>.
	/// </para>
	/// </remarks>
	/// <exception cref="InvalidOperationException">The <see cref="NullableRef{T}"/> represents a <c><see langword="null"/></c>-reference</exception>
	public readonly ref T Target
	{
		// no aggresive inlining because we potentially throw
		get
		{
			if (Unsafe.IsNullRef(ref mTarget))
			{
				failReferenceNull();
			}

			return ref mTarget;

			[DoesNotReturn]
			static void failReferenceNull() => throw new InvalidOperationException("No reference present.");
		}
	}

	/// <summary>Calls to this method are not supported</summary>
	/// <param name="obj">Not supported</param>
	/// <returns>Not supported</returns>
	/// <exception cref="NotSupportedException">always</exception>
	[Obsolete($"Calls to this method are not supported. This method will always throw an exception. Use the \"{nameof(Equals)}\" methods or the equality operators instead.")]
	[DoesNotReturn]
#pragma warning disable CS0809
	public override bool Equals([NotNullWhen(true)] object? obj) => throw new NotSupportedException("Calls to this method are not supported.");
#pragma warning disable CS0809

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly bool Equals(NullableRef<T> other) => Unsafe.AreSame(ref mTarget, ref other.mTarget);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly bool Equals(NullableRefReadOnly<T> other) => Unsafe.AreSame(ref mTarget, in other.Target);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public override int GetHashCode() { unsafe { return unchecked((nint)Unsafe.AsPointer(ref mTarget)).GetHashCode(); } }

	/// <summary>
	/// Gets an actual reference to the value of type <typeparamref name="T"/> this <see cref="NullableRef{T}"/> references, without validating if it's safely dereferencable 
	/// </summary>
	/// <returns>An actual reference to the value of type <typeparamref name="T"/> this <see cref="NullableRef{T}"/> references</returns>
	/// <remarks>
	/// <para>
	/// This method does not validate if the represented reference is safely dereferencable!
	/// Users should check the value of the <see cref="HasTarget"/> property or the value of the <see cref="IsNull"/> property first, to determine if the result of this method is safe to dereference.
	/// Use with care!
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public readonly ref readonly T GetReferenceOrNull() => ref mTarget; // we can just return mReference, since either we return a valid reference or the reference is null (Unsafe.IsNull)

	/// <inheritdoc cref="IEqualityOperators{TSelf, TOther, TResult}.operator=="/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator ==(NullableRef<T> left, NullableRef<T> right) => left.Equals(right);

	/// <inheritdoc cref="IEqualityOperators{TSelf, TOther, TResult}.operator!="/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator !=(NullableRef<T> left, NullableRef<T> right) => !(left == right);

	/// <inheritdoc cref="IEqualityOperators{TSelf, TOther, TResult}.operator=="/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator ==(NullableRef<T> left, NullableRefReadOnly<T> right) => left.Equals(right);

	/// <inheritdoc cref="IEqualityOperators{TSelf, TOther, TResult}.operator!="/>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool operator !=(NullableRef<T> left, NullableRefReadOnly<T> right) => !(left == right);

	/// <summary>
	/// Converts a <see cref="NullableRef{T}"/> to a <see cref="NullableRefReadOnly{T}"/>
	/// </summary>
	/// <param name="nullableRef">The <see cref="NullableRef{T}"/> to convert</param>
	/// <remarks>
	/// <para>
	/// The resulting <see cref="NullableRefReadOnly{T}"/> will reference the same value of <typeparamref name="T"/> <paramref name="nullableRef"/> references.
	/// This also means that, if <paramref name="nullableRef"/> represents a <c><see langword="null"/></c>-reference, the resulting <see cref="NullableRefReadOnly{T}"/> will represent a <c><see langword="null"/></c>-reference.
	/// </para>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static implicit operator NullableRefReadOnly<T>(NullableRef<T> nullableRef) => new(ref nullableRef.mTarget);
}
