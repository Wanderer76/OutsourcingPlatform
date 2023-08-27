export function formatTagValue(tag) {
    return '#' + tag.replaceAll(' ', '_').toLocaleLowerCase();
}